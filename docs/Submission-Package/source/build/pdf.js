const puppeteer=require('puppeteer-core'),fs=require('fs');
const R='D:/8th_semester/PRN232/ASM/PRN232_ASM_PROJECT/docs/Submission-Package/';
const docs=[['01-Proposal','01-proposal','Project Proposal'],['02-SRS','02-srs','Software Requirements Specification'],['03-Business-Rules','03-business-rules','Business Rules'],['04-ERD-and-Database','04-erd-database','ERD & Database Design'],['05-Architecture-Diagrams','05-architecture','Architecture Diagrams']];
const wrap=b=>`<!doctype html><html><head><meta charset="utf-8"><link rel="stylesheet" href="style.css"></head><body>${b}</body></html>`;
(async()=>{
 const br=await puppeteer.launch({executablePath:'C:/Program Files (x86)/Microsoft/Edge/Application/msedge.exe',headless:'new'});
 const out=async(file,html,title)=>{
  fs.writeFileSync(R+'source/_tmp.html',html);
  const p=await br.newPage();await p.goto('file:///'+R+'source/_tmp.html',{waitUntil:'networkidle0'});
  await p.pdf({path:R+'pdf/'+file+'.pdf',preferCSSPageSize:true,printBackground:true,displayHeaderFooter:true,
   headerTemplate:'<div></div>',footerTemplate:`<div style="font-size:8px;width:100%;padding:0 16mm;color:#777;display:flex;justify-content:space-between"><span>Longevity Diet Platform — ${title}</span><span>Page <span class="pageNumber"></span> / <span class="totalPages"></span></span></div>`});
  await p.close();console.log('pdf',file);
 };
 let all='';
 for(const [f,s,t] of docs){const b=fs.readFileSync(R+'source/'+s+'.html','utf8');all+=b.replace('<h1 class="first">','<h1>');await out(f,wrap(b),t);}
 const master=`<section class="cover"><div class="tag">PRN232 · Software Project</div><h1>Longevity Diet Platform</h1><div class="sub">Complete documentation pack</div><table><tr><td>Contents</td><td>1 Proposal · 2 SRS · 3 Business Rules · 4 ERD &amp; Database · 5 Architecture Diagrams</td></tr><tr><td>Version</td><td>1.0 (2026-11-12)</td></tr><tr><td>Team</td><td>[Team name — members and student IDs]</td></tr></table></section>`;
 await out('00-Full-Documentation-Pack',wrap(master+all.replace(/<section class="cover">/g,'<section class="cover" style="break-before:page">')),'Full Documentation Pack');
 fs.unlinkSync(R+'source/_tmp.html');
 await br.close();
})();
