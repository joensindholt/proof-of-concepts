// ES5 example
const {
  TranscribeClient,
  CreateCallAnalyticsCategoryCommand,
  StartTranscriptionJobCommand,
} = require("@aws-sdk/client-transcribe");

async function run() {
  const client = new TranscribeClient({ region: "us-east-1" });

  const command = new StartTranscriptionJobCommand({
    TranscriptionJobName: "test",
    Media: {
      MediaFileUri: "s3://josiho-us-east-1-test/speechSample.wav",
    },
    LanguageCode: "en-US",
  });

  const response = await client.send(command);

  console.log(response);

  return response;
}

run();
