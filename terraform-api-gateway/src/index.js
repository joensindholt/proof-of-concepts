exports.handler = async function (event, context) {
  console.log('EVENT: \n' + JSON.stringify(event, null, 2));

  let response = {
    statusCode: 200,
    headers: {
      'x-custom-header': 'my custom header value',
    },
    body: 'Hello terraform',
  };

  return response;
};
