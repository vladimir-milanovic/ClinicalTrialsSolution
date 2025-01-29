﻿namespace ClinicalTrials.Application.Constants;

public static class ApplicationConstants
{
    public const string JsonSchema = @"{
        ""$schema"": ""http://json-schema.org/draft-07/schema#"",
        ""title"": ""ClinicalTrialMetadata"",
        ""type"": ""object"",
        ""properties"": {
          ""trialId"": {
            ""type"": ""string""
          },
          ""title"": {
            ""type"": ""string""
          },
          ""startDate"": {
            ""type"": ""string"",
            ""format"": ""date""
          },
          ""endDate"": {
            ""type"": ""string"",
            ""format"": ""date""
          },
          ""participants"": {
            ""type"": ""integer"",
            ""minimum"": 1
          },
          ""status"": {
            ""type"": ""string"",
            ""enum"": [
              ""Not Started"",
              ""Ongoing"",
              ""Completed""
            ]
          }
        },
        ""required"": [
          ""trialId"",
          ""title"",
          ""startDate"",
          ""status""
        ],
        ""additionalProperties"": false
    }";
}
