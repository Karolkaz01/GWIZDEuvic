import boto3
import json
import time

# Initialize the DynamoDB client
dynamodb = boto3.resource('dynamodb')
table = dynamodb.Table('events')
import time

def lambda_handler(event, context):
    # Extract data from the event (assuming you're triggering this Lambda with an API Gateway and sending a JSON payload)
    id = event["id"]
    lng = event["lng"]
    lat = event["lat"]
    event_type = event["event_type"]
    animal_type = event["animal_type"]
    label = event["label"]
    breed = event["breed"]
    
    # Put the new item into the DynamoDB table
    response = table.put_item(
        Item={
            'id': id,
            'createdAt': int(time.time()),
            'lng': str(lng),
            'lat': str(lat),
            'animaL_type': animal_type,
            'event_type': event_type,
            'label': label,
            'breed': breed,
        }
    )
    
    # Return a response
    return {
        'statusCode': 200,
        'body': {'message': 'Item added successfully!'}
    }