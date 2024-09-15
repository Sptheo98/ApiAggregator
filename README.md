ASP.NET Core Web API that fetches and combines selected data from multiple external API sources

Project Name: API Aggregator
==========================================
Description:
-------------
This project is an API aggregator that fetches and combines selected data from multiple external API sources,
including weather, news, and cryptocurrency data.
It provides a single API endpoint that allows users to retrieve this aggregated data in one unified response.
Also the users can download the generated json file with the aggregated data.
This service aggregates data from multiple external APIs, including:

Weather Data: Using Open-Meteo API.

News Data: Using Mediastack API.

Cryptocurrency Data: Using CoinGecko API.


Features:
==========================================
- Fetches current weather based on geographic coordinates (latitude and longitude) , can also apply an optional date filter.
- Retrieves news articles on a given topic, with an optional date filter.
- Fetches cryptocurrency information such as current price, market cap, and volume for a given coin.


How to Run the Project:
==========================================
Prerequisites:
--------------
.NET SDK 6.0+ or later

API keys for:

Mediastack

CoinGecko

Open-Meteo: No API key required

Steps to Run:
-------------
1. Clone the repository: `git clone <repository_url>`
2. Navigate to the project directory: `cd ApiAggregator`
3. Restore dependencies: `dotnet restore`
4. Build the project: `dotnet build`
5. Run the project: `dotnet run`

If everything is set up correctly , you will get his message in your terminal :

Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
      
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]

      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]

      Content root path: C:\Users\ApiAggregator

the localhost port might be different .



Access the API:
==========================================
Once the app is running, you can access it via a browser by typing in the URL bar : http://localhost:5000/swagger
The localhost port should be the one the terminal messaged you about when you typed dotnet run .
When the page loads you should be able to see a simple swagger UI with a bar with this text : 
GET /api/Aggregate
If you press it you should be able to see some fields you have to fill with parameters.

Parameters:
-----------
- latitude (double) – Required: Latitude of the location for weather data.
- longitude (double) – Required: Longitude of the location for weather data.
- coinId (string) – Required: The ID of the cryptocurrency (e.g., "bitcoin").
- topic (string) – Required: The topic for news (e.g., "technology").
- date (string) – Optional: Date filter for news and weather data (format: YYYY-MM-DD).

if you fill them and press execute you should see this :
Example Request:
  'http://localhost:5000/api/Aggregate?latitude=22&longitude=22&coinId=tether&topic=sports' \

Example server Response:
-----------------
{
  "crypto": { ... },
  "news": { ... },
  "weather": { ... }
}


Project Structure:
==========================================
- /Controllers: Contains the AggregateController, which handles the API requests.
- /Services: Contains the services that interact with external APIs (WeatherService, CryptoService, NewsService).
- /Models: Contains the models that represent the structure of the external API responses and the aggregated data.

Error Handling:
=====================================
- 400 BadRequest: If required parameters are missing or invalid.
- 500 InternalServerError: If an error occurs while fetching data from any of the external services.


Dependencies and External APIs:
==========================================
- External APIs:
  - Open Meteo API
  - Mediastack News API
  - CoinGecko Cryptocurrency API.

- Packages:
  - System.Net.Http – For making HTTP requests to external APIs.
  - System.Text.Json – For parsing JSON responses.
