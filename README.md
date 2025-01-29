Clinical Trial Metadata System
=============================
Functional Requirements:
1. File Upload Constraints:
● File Size Limitation: Restrict the size of uploaded files to prevent excessively
large uploads.
● File Type Validation: Accept only files with a .json extension.
2. JSON Validation:
● Schema Validation: Validate the uploaded JSON file against the provided
JSON schema.
● Schema Storage: Store the JSON schema as an embedded resource or in a
database of your choice.
3. Data Conformance and Storage:
● Database Schema Alignment: Ensure the data conforms to the database
schema before storage.
● Data Mapping: Map the JSON data to corresponding classes and tables using
an Object-Relational Mapping (ORM) tool like Entity Framework (EF) Code First.
4. Data Retrieval Endpoints:
● Get Specific Record by ID: Implement an endpoint to retrieve a specific record
using its unique identifier.
● Filtering Support: Allow filtering of records based on query parameters (e.g.,
status).
5. Testing:
● Unit and Integration Tests: Write unit or integration tests for critical
components to ensure reliability and maintainability.
6. API Documentation:
● Swagger/ OpenAPI: Utilize Swagger/ OpenAPI to generate interactive API
documentation.
● README File: If necessary, provide a README file with setup instructions and
usage guidelines.
7. Architectural Design:
● Clean Architecture: Utilize Clean Architecture principles to separate concerns,
promote scalability, and facilitate maintainability.
● Code Quality: Write clean, maintainable, and testable code following industry
best practices.
8. Containerization:
● Docker: Containerize the application using Docker to ensure consistency
across different environments and ease of deployment.