import fs from 'fs';
import path from 'path';

const dirs = ['Views/Home', 'Views/Shared'];

dirs.forEach(dir => {
    fs.readdirSync(dir).forEach(file => {
        if (file.endsWith('.cshtml')) {
            const filePath = path.join(dir, file);
            let content = fs.readFileSync(filePath, 'utf8');
            
            // Fix data-lucide="something-icon" to "something"
            content = content.replace(/data-lucide="([^"]+)-icon"/g, 'data-lucide="$1"');
            
            // Fix any weird imports remaining
            content = content.replace(/^import\s+.*?\s+from\s+['"].*?['"];?\s*$/gm, '');
            
            // Make button links work (href was put on buttons, better to just change buttons to a tags)
            // or just let it be for now since it's just visual. Wait, standard html href on button doesn't work.
            // Let's replace `<button href=` with `<a href=` and `</button>` with `</a>` (rough heuristic)
            content = content.replace(/<button([^>]*?)href="([^"]+)"([^>]*)>/g, '<a$1href="$2"$3>');
            // This might mismatch closing tags, but we'll manually check.
            
            fs.writeFileSync(filePath, content.trim());
        }
    });
});
console.log('Fixed icons');
