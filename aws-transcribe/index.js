const { TranscribeStreamingClient, StartStreamTranscriptionCommand } = require('@aws-sdk/client-transcribe-streaming');
const { createReadStream } = require('fs');
const { join } = require('path');

const audio = createReadStream(join('C:\\Users\\joens\\Downloads', 'speechSample.wav'), { highWaterMark: 1024 * 16 });

const LanguageCode = 'en-US';
const MediaEncoding = 'pcm';
const MediaSampleRateHertz = '48000';
async function startRequest() {
  const client = new TranscribeStreamingClient({
    region: 'us-east-1',
  });

  const params = {
    LanguageCode,
    MediaEncoding,
    MediaSampleRateHertz,
    AudioStream: (async function* () {
      for await (const chunk of audio) {
        yield { AudioEvent: { AudioChunk: chunk } };
      }
    })(),
  };
  const command = new StartStreamTranscriptionCommand(params);
  // Send transcription request
  const response = await client.send(command);
  // Start to print response
  try {
    for await (const event of response.TranscriptResultStream) {
      console.log(JSON.stringify(event));
    }
  } catch (err) {
    console.log('error');
    console.log(err);
  }
}
startRequest();
