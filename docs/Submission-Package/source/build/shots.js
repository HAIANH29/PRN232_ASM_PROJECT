const puppeteer=require('puppeteer-core');
const OUT='D:/8th_semester/PRN232/ASM/PRN232_ASM_PROJECT/docs/Submission-Package/screenshots/';
const B='http://localhost:5000';
(async()=>{
 const br=await puppeteer.launch({executablePath:'C:/Program Files (x86)/Microsoft/Edge/Application/msedge.exe',headless:'new',defaultViewport:{width:1440,height:900}});
 const sh=async(p,name,full=true)=>{try{await p.screenshot({path:OUT+name+'.png',fullPage:full});console.log('ok',name)}catch(e){console.log('fail',name,e.message)}};
 const go=async(p,u)=>{await p.goto(B+u,{waitUntil:'networkidle2',timeout:60000})};
 const login=async(p,e,pw)=>{await go(p,'/Auth/Login');await p.type('input[name=Email]',e);await p.type('input[name=Password]',pw);await Promise.all([p.waitForNavigation({waitUntil:'networkidle2'}),p.click('form button[type=submit], form input[type=submit]')])};
 // user
 let p=await br.newPage();
 await go(p,'/');await sh(p,'01-home-guest');
 await go(p,'/Auth/Login');await sh(p,'02-login');
 await login(p,'user@longevity.local','User@123456');
 for(const [u,n] of [['/','03-home-user'],['/Knowledge','04-knowledge'],['/Knowledge/Search','05-semantic-search'],['/Regimens','06-regimens'],['/MealPlans','07-meal-plans'],['/Recommendations','08-ai-recommendations'],['/Tracking','09-tracking'],['/Auth/Profile','10-profile']]){await go(p,u);await sh(p,n)}
 // details pages via first link
 for(const [u,sel,n] of [['/Regimens','a[href*="/Regimens/Details"]','06b-regimen-details'],['/MealPlans','a[href*="/MealPlans/Details"]','07b-meal-plan-details']]){await go(p,u);const h=await p.$eval(sel,a=>a.getAttribute('href')).catch(()=>null);if(h){await p.goto(B+h,{waitUntil:'networkidle2'});await sh(p,n)}}
 await p.close();
 // admin
 p=await br.newPage();await login(p,'admin@longevity.local','Admin@123456');
 for(const [u,n] of [['/AdminKnowledge','11-admin-knowledge'],['/AdminBookSources','12-admin-book-sources'],['/AdminRegimens','13-admin-regimens']]){await go(p,u);await sh(p,n)}
 const h=await p.$eval('a[href*="/AdminBookSources/Details"]',a=>a.getAttribute('href')).catch(()=>null);
 if(h){await p.goto(B+h,{waitUntil:'networkidle2'});await sh(p,'12b-admin-candidates-review')}
 // language VI
 await p.goto(B+'/Language/Set?culture=vi',{waitUntil:'networkidle2'}).catch(()=>{});
 await br.close();
})();
