## The Task

Ensuring tha acceptance criteria are met, build a C# Web API that connect to an instance of a database and persists the contents of the merter reading CSV file.

Please seed the accounts data into your chosen data storage technology and validate meter read data against the accounts.

## User Story

As an energy company account manager, I want to be able to load a CSV file of customer meter readings so tat we can monitor their energy consumption and charge them accordingly.

## Acceptance Criteria

- Create the following endpoint: POST /meter-reading-uploads
- The endpoint should be able to process a CSV of meter readings.
- Each entry in the CSV should be validated and if valid, stored in DB.
- After processing, the number of successful/failed readings should be returned.

Validation

- You should not be able to load the same entry twice
- A meter reading must be associated with an account ID to be deemed valid
- Reading values should be in the format NNNNN

Nice to Have

- Create a client in the technology of your choosing to consume the API
- When an account has an existing read, ensure the new read isn't older than the existing read

## Materials

[Meter_reading.csv](csv/Meter_reading.csv)

[Test_accounts](csv/Test_accounts.csv) contains a list of test customers along with their respective account IDs
