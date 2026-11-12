const puppeteer=require('puppeteer-core'),fs=require('fs'),path=require('path');
const OUT='D:/8th_semester/PRN232/ASM/PRN232_ASM_PROJECT/docs/Submission-Package/diagrams/';
const MMD='D:/8th_semester/PRN232/ASM/PRN232_ASM_PROJECT/docs/Submission-Package/source/mmd/';
fs.mkdirSync(MMD,{recursive:true});
const parts=fs.readFileSync('all.mmd','utf8').split(/^=== /m).filter(Boolean);
const lib=fs.readFileSync(path.join(__dirname,'node_modules/mermaid/dist/mermaid.min.js'),'utf8');
(async()=>{
 const br=await puppeteer.launch({executablePath:'C:/Program Files (x86)/Microsoft/Edge/Application/msedge.exe',headless:'new',defaultViewport:{width:2000,height:1200,deviceScaleFactor:2}});
 for(const part of parts){
  const i=part.indexOf('\n');const name=part.slice(0,i).trim(),code=part.slice(i+1);
  fs.writeFileSync(MMD+name+'.mmd',code);
  const p=await br.newPage();
  await p.setContent('<html><body style="margin:0;background:#fff"><div id="g" style="display:inline-block;padding:16px"></div></body></html>');
  await p.addScriptTag({content:lib});
  try{
   await p.evaluate(async(code)=>{mermaid.initialize({startOnLoad:false,theme:'base',themeVariables:{primaryColor:'#e6f2ec',primaryBorderColor:'#2f7d57',primaryTextColor:'#143d2a',lineColor:'#4a5d53',fontFamily:'Segoe UI, Arial',fontSize:'15px'},securityLevel:'loose'});const {svg}=await mermaid.render('x',code);document.getElementById('g').innerHTML=svg;document.body.style.zoom=1.7;},code);
   const el=await p.$('#g');await el.screenshot({path:OUT+name+'.png'});console.log('ok',name);
  }catch(e){console.log('FAIL',name,e.message.slice(0,200))}
  await p.close();
 }
 await br.close();
})();
