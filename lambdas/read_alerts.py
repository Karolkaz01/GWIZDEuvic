import boto3
import json
import time

# Initialize the DynamoDB client
dynamodb = boto3.resource('dynamodb')
table = dynamodb.Table('events')
import time

def lambda_handler(event, context):
    data = table.scan()
    
  
    # Return a response
    return {
        'statusCode': 200,
        'body': data
    }