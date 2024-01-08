const puppeteer = require('puppeteer-core');

(async () => {
  const browser = await puppeteer.launch({
    executablePath: 'C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe',
    headless: true,
    //slowMo: 250,
    defaultViewport: { width: 1280, height: 1024 },
  });
  const page = await browser.newPage();

  console.log('Opening main page');
  await page.goto('https://app.semantechx.com');

  console.log('Signing in');
  await signIn(page);

  console.log('Switching account');
  //await switchAccount(page); //Glennie

  await sleep(1000);

  console.log('Determining entity');
  //const entity = await findFirstEntity(page);
  const entity = 'FACSAria';

  console.log('Opening report page');
  await page.goto('http://localhost:4200/report/' + entity);
  await sleep(4000);

  console.log('Generating pdf');
  await page.pdf({ path: 'hn.pdf', format: 'A4' });

  console.log('Closing browser');
  await browser.close();
})();

async function findFirstEntity(page) {
  const link = await page.waitForSelector('app-dashboard-list-table a');
  const href = await link.getProperty('href');
  const entity = (await href.jsonValue()).split('/').pop();
  return entity;
}

async function switchAccount(page) {
  const accountButton = await page.waitForSelector('.account-button', { visible: true });
  await accountButton.click();

  const personSelector = await page.waitForSelector('.account-menu__user-name select', { visible: true });
  await personSelector.select('0: c67ecc97-1388-48db-b028-c1d57e374939');
}

async function signIn(page) {
  const usernameInput = await page.waitForSelector('.layout-right [formcontrolname="username"]', {
    visible: true,
  });

  await usernameInput.type('joensindholt@gmail.com');
  await (await page.$('.layout-right [formcontrolname="password"]')).type('Glennie!!24');
  await (await page.$('.layout-right [type="submit"]')).click();
}

async function sleep(delay) {
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve();
    }, delay);
  });
}
