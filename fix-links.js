import fs from 'fs';
import path from 'path';

const dirs = ['Views/Home', 'Views/Shared'];

dirs.forEach(d => {
    fs.readdirSync(d).forEach(f => {
        if(f.endsWith('.cshtml')){
            let c = fs.readFileSync(path.join(d,f), 'utf8');
            c = c.replace(/<a([^>]*?)href="([^"]+)"([^>]*)>([\s\S]*?)<\/button>/g, '<a$1href="$2"$3>$4</a>');
            fs.writeFileSync(path.join(d,f), c);
        }
    });
});
console.log('Fixed button tags');
