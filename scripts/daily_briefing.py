#!/usr/bin/env python3
"""每日行业资讯早报 - 多源聚合"""

import urllib.request, urllib.parse, re, time, sys
from datetime import datetime

TIMEOUT = 15
HEADERS = {"User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36"}

SITES = [
    ("http://www.aircon.com.cn/", "艾肯网", "暖通"),
    ("https://www.hvacrhome.com/", "暖通家", "暖通"),
    ("https://www.hvacr.com.cn/", "暖通空调在线", "暖通"),
    ("http://bao.hvacr.cn/", "制冷快报", "暖通"),
    ("https://www.cidc.org.cn/", "CIDC数据中心委员会", "数据中心"),
    ("http://www.odcc.org.cn/", "ODCC开放数据中心", "数据中心"),
    ("http://www.dcworld.cn/", "数据中心世界", "数据中心"),
    ("http://www.chinacraa.org/", "中国制冷空调工业协会", "制冷"),
    ("http://www.chinaiol.com/", "产业在线", "制冷"),
    ("http://www.chpa.org.cn/", "中国热泵专业委员会", "热泵"),
    ("https://m.eeworld.com.cn/", "电子工程世界", "技术"),
    ("https://www.itherm.cn/", "iTherM热管理", "技术"),
    ("https://www.accessen.cn/", "上海艾克森", "板换"),
    ("https://www.shphe.com/", "上海板换机械", "板换"),
    ("https://www.alfalaval.cn/", "阿法拉伐中国", "板换"),
    ("https://www.envicool.com/news/", "英维克", "液冷企业"),
    ("https://ieeexplore.ieee.org/search/searchresult.jsp?queryText=data%20center%20cooling%20liquid%20heat%20exchanger%20heat%20pump", "IEEE Xplore", "学术"),
]

KW_ZH = ["液冷","冷却","换热器","板式","板换","热泵","制冷","空调",
         "节能","储能","余热","温差","压缩","蒸发","冷凝","暖通",
         "数据中心","算力","芯片散热","浸没","冷板","热管理"]

KW_EN = ["liquid cooling","data center","heat exchanger","plate heat",
         "heat pump","thermal management","cooling","chiller",
         "brazed","immersion cooling","cold plate","hvac"]

def relevant(title):
    t = title.lower()
    return any(k.lower() in t for k in KW_ZH + KW_EN)

def fetch(url, timeout=TIMEOUT):
    req = urllib.request.Request(url, headers=HEADERS)
    try:
        with urllib.request.urlopen(req, timeout=timeout) as r:
            raw = r.read()
        for enc in ["utf-8","gbk","gb2312","gb18030"]:
            try: return raw.decode(enc)
            except: pass
        return raw.decode("utf-8", errors="replace")
    except:
        return ""

def get_links(html, base):
    out = []
    for m in re.finditer(r'<a[^>]*href="([^"]+)"[^>]*>(.{10,200}?)</a>', html, re.DOTALL|re.I):
        url, title = m.group(1), re.sub(r"<[^>]+>","",m.group(2)).strip()
        title = re.sub(r"\s+"," ",title)
        if not title or len(title)<8 or any(x in title for x in ["下一页","首页","上一页","更多"]):
            continue
        if url.startswith("/"): url = base.rstrip("/")+url
        elif not url.startswith("http"): continue
        if relevant(title):
            out.append({"title":title,"url":url})
    for m in re.finditer(r'<h3[^>]*>(.{10,200}?)</h3>', html, re.DOTALL):
        t = re.sub(r"<[^>]+>","",m.group(1)).strip()
        if relevant(t) and len(t)>8 and not any(o["title"]==t for o in out):
            out.append({"title":t,"url":base})
    return out[:5]

def crawl():
    all_news = []
    sys.stderr.write(f"抓取 {len(SITES)} 站...\n")
    for url,name,cat in SITES:
        html = fetch(url)
        if not html: continue
        links = get_links(html, url)
        for it in links:
            it["source"] = name; it["category"] = cat
        all_news.extend(links)
        if links: sys.stderr.write(f"  {name}: {len(links)}\n")
        time.sleep(0.4)
    seen = set(); uniq = []
    for n in all_news:
        if n["title"] not in seen:
            seen.add(n["title"]); uniq.append(n)
    return uniq

def clean_text(html_str):
    t = re.sub(r"<script[^>]*>.*?</script>","",html_str,flags=re.DOTALL|re.I)
    t = re.sub(r"<style[^>]*>.*?</style>","",t,flags=re.DOTALL|re.I)
    t = re.sub(r"<[^>]+>"," ",t)
    t = re.sub(r"\s+"," ",t).strip()
    chars = []
    for c in t:
        if '\u4e00' <= c <= '\u9fff' or c.isalnum() or c.isspace():
            chars.append(c)
        elif c in '，。、；：！？…—.,;:!?%-()[]【】《》"\'':
            chars.append(c)
    return ''.join(chars)[:250].strip()

def report(news):
    today = datetime.now().strftime("%Y年%m月%d日")
    lines = [f"## 行业资讯早报 — {today}", "",
             f"> 扫描 {len(SITES)} 站，筛选 {len(news)} 条相关资讯", ""]
    
    lines.append("### 今日速览")
    top = [n for n in news if any(k in n["title"] for k in ["液冷","热泵","板式","换热器","数据中心"])] or news
    for i,n in enumerate(top[:3]):
        lines.append(f"{i+1}. **[{n['title'][:80]}]({n['url']})** — {n['source']}")
    lines.append("")
    
    lines.append("### 深度解读")
    best = top[0] if top else None
    if best:
        lines.append(f"**{best['title'][:100]}**")
        lines.append(f"来源: {best['source']} | [阅读原文]({best['url']})")
        c = fetch(best["url"])
        if c:
            txt = clean_text(c)
            if txt: lines.append(f"> {txt}...")
    else:
        lines.append("*今日暂无深度解读内容*")
    lines.append("")
    
    lines.append("### 行业数据")
    di = [n for n in news if any(k in n["title"] for k in ["报告","数据","增长","规模","预测","%","亿"])]
    if di:
        for d in di[:5]:
            lines.append(f"- [{d['title'][:60]}]({d['url']}) — {d['source']}")
    else:
        lines.append("*今日暂无行业数据*")
    lines.append("")
    
    lines.append("### 明日关注")
    lines.append("- 液冷技术在AI算力中心的渗透进展")
    lines.append("- 热泵行业最新政策及标准动向")
    return "\n".join(lines)

if __name__ == "__main__":
    news = crawl()
    if not news:
        news = [{"title":"今日网络访问受限，请检查网络连接","url":"#","source":"系统"}]
    print(report(news))
