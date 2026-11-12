const puppeteer=require('puppeteer-core');
const OUT='D:/8th_semester/PRN232/ASM/PRN232_ASM_PROJECT/docs/Submission-Package/screenshots/';
const B='http://localhost:5000';
(async()=>{
 const br=await puppeteer.launch({executablePath:'C:/Program Files (x86)/Microsoft/Edge/Application/msedge.exe',headless:'new',defaultViewport:{width:1440,height:900}});
 const ctx=await br.createBrowserContext();const p=await ctx.newPage();
 const sh=async(n)=>{await p.screenshot({path:OUT+n+'.png',fullPage:true});console.log('ok',n,p.url())};
 await p.goto(B+'/Auth/Login',{waitUntil:'networkidle2'});
 await p.type('input[name=Email]','admin@longevity.local');await p.type('input[name=Password]','Admin@123456');
 await Promise.all([p.waitForNavigation({waitUntil:'networkidle2'}),p.click('form button[type=submit]')]);
 for(const [u,n] of [['/AdminKnowledge','11-admin-knowledge'],['/AdminBookSources','12-admin-book-sources'],['/AdminRegimens','13-admin-regimens']]){await p.goto(B+u,{waitUntil:'networkidle2'});await sh(n)}
 const h=await p.$eval('a[href*="/AdminBookSources/Details"]',a=>a.getAttribute('href')).catch(()=>null);
 if(h){await p.goto(B+h,{waitUntil:'networkidle2'});await sh('12b-admin-candidates-review')}
 await br.close();
})();
