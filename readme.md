# Jobs Marketplace API

A RESTful API for a job outsourcing marketplace, built with ASP.NET Core and Entity Framework Core.

## Prerequisites
* .NET 10.0 SDK
* Postman
* Visual Studio 2026 (Optional)

## How to Run

### Option 1: Using the Command Line
1. **Launch the API:**
   Run the following command in your terminal from the project root:
   
   dotnet run
   
Note: Pay attention to the console output to see which port the application is listening on (e.g., http://localhost:5249).

Option 2: Using Visual Studio 2026
Open the .sln file in Visual Studio 2026.

I've created a single project with Folders to separate the layers/namespaces to make it simple to run and test

Press F5 or click the Play button to build and launch the application.

The IDE will automatically launch the service; note the port in the browser URL bar or console window.

Database
The SQLite database (jobsmarketplace.db) is already included in the project. You do not need to run migrations. If you need to reset the data, simply delete the .db file and run dotnet ef database update in your terminal.

Testing the API
I have organized the testing process into a Postman collection.

Important: Postman Configuration
To successfully test the endpoints, you must update the baseUrl variable in your Postman environment:

Open your Postman Environment.

Set the baseUrl variable to match the port displayed in your terminal or IDE (e.g., http://localhost:5249).

Use {{baseUrl}} in your request URLs, for example: {{baseUrl}}/api/jobs/1.

Key API Workflows
Jobs: Retrieve available jobs (with pagination), create new job postings or update existing records.

JobOffers: Contractors can POST new offers to a specific jobId.

Once a customer identifies a suitable offer, they use the POST /api/joboffers/{id}/accept endpoint to finalize the contract. This operation is transactional, ensuring the job is linked to the contractor and the offer status is updated simultaneously.