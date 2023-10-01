import openai
import dotenv
import os
import json
import requests
import boto3
import time

dotenv.load_dotenv()
openai.api_key = os.environ["OPENAI_API_KEY"]
feed_id = os.environ["FEED_ID"]
fb_access_token = os.environ["FB_ACCESS_TOKEN"]
dynamodb = boto3.resource('dynamodb')
table = dynamodb.Table('events')

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
    "wild": "dziki",
    "marten": "kuna"
}


def sync_post(id, message):

    response = openai.ChatCompletion.create(
        model="gpt-4",
        messages=[
            {
                "role": "system",
                "content": """
            Given post information create json with the following structure:
            {
            "location":  "street or characteristic place, concatenate it with city: Kraków",
            "event_type":  "reported | lost",
            "animaL_type": "domestic | wild",
            "label": "type of animal",
            "breed": "based on content or leave empty if wild animal"
            }      
        """,
            },
            {
                "role": "user",
                "content": "Właśnie widziałam kunę. Na ulicy Czarnowiejskiej. Uważajcie, bo przegryzie Wam kable w aucie",
            },
            {
                "role": "system",
                "content": """
            {
            "location":  "Czarnowiejska, Kraków",
            "event_type":  "reported",
            "animaL_type": "wild",
            "breed": "",
            "label": "marten"
            }         
            """,
            },
            {
                "role": "user",
                "content": "Zgubiłem psa rasy husky w okolicy Reymonta. Proszę o pomoc!",
            },
            {
                "role": "system",
                "content": """
            {
            "location":  "Reymonta, Kraków",
            "event_type":  "lost",
            "animaL_type": "domestic",
            "breed": "husky",
            "label": "dog"
            }         
            """,
            },            
            {
                "role": "user",
                "content": f"""{message}""",
            },
        ],
    )


    answer = response["choices"][0]["message"]["content"].replace("\n", "").strip()
    print(answer)
    json_answer = json.loads(answer)

    location_name = json_answer["location"]


    def get_coordinates_from_location(location_name):
        # Nominatim API endpoint
        endpoint = "https://nominatim.openstreetmap.org/search"
        
        # Parameters for the request
        params = {
            "q": location_name,
            "format": "json",
            "limit": 1
        }
        
        # Send the request
        response = requests.get(endpoint, params=params)
        
        # Check if the response is valid
        if response.status_code == 200:
            data = response.json()
            if data:
                latitude = data[0]["lat"]
                longitude = data[0]["lon"]
                return latitude, longitude
            else:
                return None, None
        else:
            print(f"Error {response.status_code}: {response.text}")
            return None, None


    lat, lon = get_coordinates_from_location(location_name)

    json_answer["lat"] = lat
    json_answer["lng"] = lon
    json_answer["id"] = id
    for k in json_answer.keys():
        if json_answer.get(k, None):
            k2 = json_answer.get(k, None)            
            if k2 in translation_dict.keys():
                json_answer[k] = translation_dict[k2]                             
    return json_answer



feed = requests.get(f"https://graph.facebook.com/v18.0/{feed_id}/feed?access_token={fb_access_token}")
feed_data = feed.json()["data"]

def check_if_event_exists(event_id):  
    # Define the query parameters
    params = {
        'KeyConditionExpression': 'id = :id',
        'ExpressionAttributeValues': {
            ':id': event_id
        }
    }
    # Execute the query
    response = table.query(**params)
    return len(response['Items']) > 0


i = 0
first_feed_data = feed_data[i]
event_in_table = check_if_event_exists(first_feed_data["id"])
print(f"{first_feed_data} ")
if not event_in_table:
    data_to_save = sync_post(first_feed_data["id"], first_feed_data)
    data_to_save["id"] = first_feed_data["id"]
    data_to_save["createdAt"] = int(time.time())    
    print(data_to_save)
    response = table.put_item(Item=data_to_save)
    
