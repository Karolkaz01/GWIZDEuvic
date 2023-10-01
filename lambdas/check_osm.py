import requests

def check_wildlife_area_nearby(lat, lon, radius=200):
    """
    Check if there's an area nearby where wild animals might appear.
    
    Parameters:
    - lat: Latitude of the location
    - lon: Longitude of the location
    - radius: Radius around the location to search for such areas (in meters)
    
    Returns:
    - True if such an area is found, False otherwise
    """
    
    # Define the Overpass API endpoint
    overpass_url = "https://overpass-api.de/api/interpreter"
    
    # Define the Overpass QL query
    overpass_query = f"""
    [out:json];
    (
      node["natural"~"wood|scrub|heath|grassland|wetland|sand|mud"](around:{radius},{lat},{lon});
      way["natural"~"wood|scrub|heath|grassland|wetland|sand|mud"](around:{radius},{lat},{lon});
      relation["natural"~"wood|scrub|heath|grassland|wetland|sand|mud"](around:{radius},{lat},{lon});
      
      node["landuse"="meadow"](around:{radius},{lat},{lon});
      way["landuse"="meadow"](around:{radius},{lat},{lon});
      relation["landuse"="meadow"](around:{radius},{lat},{lon});
      
      node["leisure"="nature_reserve"](around:{radius},{lat},{lon});
      way["leisure"="nature_reserve"](around:{radius},{lat},{lon});
      relation["leisure"="nature_reserve"](around:{radius},{lat},{lon});
      
      node["boundary"~"national_park|protected_area"](around:{radius},{lat},{lon});
      way["boundary"~"national_park|protected_area"](around:{radius},{lat},{lon});
      relation["boundary"~"national_park|protected_area"](around:{radius},{lat},{lon});
    );
    out body;
    >;
    out skel qt;
    """
    
    # Make the API request
    response = requests.get(overpass_url, params={'data': overpass_query})
    data = response.json()
    
    # Check if any relevant areas are found
    return len(data['elements'])
    # return data['elements']



cords_urban = [
    [50.06764,19.99149], # krakow tauron arena
    [50.0699, 19.9654], # krakow urbna area 
]
cords_forest = [
    [50.05882, 19.85712], # in forest
    [50.05925, 19.86759], # next to forest
    [50.05465, 19.87264], # next to forest
]

# Example usage
for lat, lon in cords_urban + cords_forest:    
    osm_data = check_wildlife_area_nearby(lat, lon)
    print(osm_data)