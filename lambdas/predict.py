import json
from PIL import Image, ImageDraw


import boto3

labels_mapping = {"pig": "boar"}

allowed_labels_wild_animals = [
    "boar",
    "pig",
    "deer",
    "bird",
    "eagle",
    "falcon",
    "fox",
    "gazelle",
    "giraffe",
    "gorilla",
    "hippo",
    "hyena",
    "kangaroo",
    "koala",
    "leopard",
    "lion",
    "monkey",
    "panda",
    "penguin",
    "rhino",
    "seal",
    "tiger",
    "wolf",
]

allowed_domestic_animals = ["cat", "dog"]

allowed_labels_dog_breeds = [
    "husky",
    "pug",
    "poodle",
    "rottweiler",
    "shiba",
    "shih",
    "terrier",
]

allowed_labels_cat_breeds = [
    "abyssinian",
    "bengal",
    "birman",
    "bombay",
    "british",
    "burmese",
    "egyptian",
    "himalayan",
    "javanese",
    "korat",
    "maine",
    "manx",
    "persian",
    "ragdoll",
    "russian",
    "siamese",
    "sphynx",
]

translation_dict = {
    "boar": "dzik",
    "pig": "świnia",
    "deer": "jeleń",
    "bird": "ptak",
    "eagle": "orzeł",
    "falcon": "sokół",
    "fox": "lis",
    "gazelle": "gazela",
    "giraffe": "żyrafa",
    "gorilla": "goryl",
    "hippo": "hipopotam",
    "hyena": "hiena",
    "kangaroo": "kangur",
    "koala": "koala",
    "leopard": "lampart",
    "lion": "lew",
    "monkey": "małpa",
    "panda": "panda",
    "penguin": "pingwin",
    "rhino": "nosorożec",
    "seal": "foka",
    "tiger": "tygrys",
    "wolf": "wilk",
    "cat": "kot",
    "dog": "pies",
    "husky": "husky",
    "pug": "mops",
    "poodle": "pudel",
    "rottweiler": "rottweiler",
    "shiba": "shiba inu",
    "shih": "shih tzu",
    "terrier": "terier",
    "abyssinian": "abisyński",
    "bengal": "bengalski",
    "birman": "birmański",
    "bombay": "bombajski",
    "british": "brytyjski",
    "burmese": "birmański",
    "egyptian": "egipski",
    "himalayan": "himalajski",
    "javanese": "jawajski",
    "korat": "korat",
    "maine": "maine coon",
    "manx": "manx",
    "persian": "perski",
    "ragdoll": "ragdoll",
    "russian": "rosyjski",
    "siamese": "syjamski",
    "sphynx": "sfinks",
    "domestic": "domowy",
    "wild": "dziki"
}


s3_client = boto3.client('s3')

def get_label_info(label):
    instances = []

    for instance in label["Instances"]:
        instances.append(
            {
                "name": label["Name"].lower(),
                "bbox": {
                    "top": float(instance["BoundingBox"]["Top"]),
                    "left": float(instance["BoundingBox"]["Left"]),
                    "width": float(instance["BoundingBox"]["Width"]),
                    "height": float(instance["BoundingBox"]["Height"]),
                },
                "confidence": float(instance["Confidence"]),
            }
        )
    return instances


def detect_labels(photo, bucket):
    session = boto3.Session()
    client = session.client("rekognition")

    response = client.detect_labels(
        Image={
            "S3Object": {
                "Bucket": bucket,
                "Name": photo,
            }
        },
        MaxLabels=10,
        Settings={
            "GeneralLabels": {
                "LabelCategoryInclusionFilters": [
                    "Animals and Pets",
                ],
            }
        },
    )
    final_labels = []
    picked_label_name = None
    label_type = None
    picked_breed_name = None
    for label in response["Labels"]:
        label_name = label["Name"].lower()
        if label_name in allowed_labels_wild_animals:
            picked_label_name = label_name
            label_type = "wild"
            break
        elif label_name in allowed_domestic_animals:
            picked_label_name = label_name
            label_type = "domestic"
            for breed in response["Labels"]:
                breed_name = breed["Name"].lower()
                if (
                    breed_name in allowed_labels_dog_breeds
                    or breed_name in allowed_labels_cat_breeds
                ):
                    picked_breed_name = breed_name
                    break
            break
        
    for label in response["Labels"]:
        label_name = label["Name"].lower()
        primary_labels = get_label_info(label)
        if len(primary_labels) > 0:
            for instance in primary_labels:
                instance["type"] = translation_dict[label_type]
                instance["name"] = translation_dict[picked_label_name]
                if picked_breed_name:
                    instance["breed"] = translation_dict[picked_breed_name]
                final_labels.append(instance)
            break
    
    return final_labels


def lambda_handler(event, context):
    print(event)
    bucket = "hack-yeah-animals"
    id = event["id"]
    photo = f"animals/{id}.jpeg"
    photo_replaced = f"animals/{id}_predicted.jpeg"
    labels = detect_labels(photo, bucket)
    # TODO implement
    print(labels)
    
    s3_client.download_file(bucket, photo, '/tmp/image.jpeg')
    
    # Open the image and draw a rectangle
    with Image.open('/tmp/image.jpeg') as img:
        draw = ImageDraw.Draw(img)
        
        try:
            for l in labels:
                bbox = l["bbox"]
                top, left, width, height = bbox["top"], bbox["left"], bbox["width"], bbox["height"],     
                top, left, width, height = top*img.height, left*img.width, width*img.width, height*img.height
                draw.rectangle([(left, top), (left+width, top+height)], outline ='red')
        except:
            raise Exception(str(l))
        # Save the modified image
        img.save('/tmp/modified_image.jpeg')
    
    # Upload the modified image back to S3
    s3_client.upload_file('/tmp/modified_image.jpeg', bucket, photo_replaced)
    
    return {
        "statusCode": 200,
        "body": {"image_src": event["id"], "labels": labels},
    }
