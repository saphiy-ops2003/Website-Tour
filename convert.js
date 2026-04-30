import fs from 'fs';
import path from 'path';

const srcDir = 'react-original/src/pages';
const compDir = 'react-original/src/components';
const outDir = 'Views/Home';
const sharedDir = 'Views/Shared';

// Ensure dirs exist
if (!fs.existsSync(outDir)) fs.mkdirSync(outDir, { recursive: true });
if (!fs.existsSync(sharedDir)) fs.mkdirSync(sharedDir, { recursive: true });

function convertToHtml(content, isComponent = false) {
    // Basic regex conversions
    let html = content;
    
    // Remove imports
    html = html.replace(/import.*?['"].*?['"];/g, '');
    html = html.replace(/export default function \w+\(.*?\)\s*{/g, '');
    html = html.replace(/return\s*\(/, '');
    
    // Remove ending braces of the functional component
    html = html.replace(/\);\s*}\s*$/g, '');

    // Convert className to class
    html = html.replace(/className=/g, 'class=');

    // Convert motion components
    html = html.replace(/<motion\.(\w+)[^>]*>/g, '<$1>');
    html = html.replace(/<\/motion\.(\w+)>/g, '</$1>');

    // Convert Lucide icons: <IconName className="..." /> -> <i data-lucide="icon-name" class="..."></i>
    // Find all <XyzIcon .../> or <Xyz .../>
    html = html.replace(/<([A-Z]\w+)(Icon)?\s*([^>]*?)\/?>/g, (match, p1, p2, p3) => {
        // Convert PascalCase to kebab-case
        const iconName = p1.replace(/([a-z0-9])([A-Z])/g, '$1-$2').toLowerCase();
        return `<i data-lucide="${iconName}" ${p3}></i>`;
    });
    
    html = html.replace(/<\/([A-Z]\w+)(Icon)?>/g, '</i>');

    // Remove React event handlers
    html = html.replace(/onClick=\{[^\}]+\}/g, '');
    html = html.replace(/onSubmit=\{[^\}]+\}/g, '');
    html = html.replace(/onChange=\{[^\}]+\}/g, '');

    // Replace setView('...') with href
    html = html.replace(/onClick=\{\(\) => setView\('(.+?)'\)\}/g, 'href="/Home/$1"');
    
    // Sometimes there are template literals or dynamic classes: class={`...`}
    // Simplify them for now
    html = html.replace(/class=\{\`([^\`]+)\`\}/g, 'class="$1"');
    html = html.replace(/class=\{.*?\}/g, 'class=""');

    // Replace any remaining {} blocks (like state variables)
    html = html.replace(/\{[^\}]+\}/g, '');

    // Fix unclosed tags or react specific stuff
    html = html.replace(/htmlFor=/g, 'for=');

    return html.trim();
}

// Convert Pages
fs.readdirSync(srcDir).forEach(file => {
    if (file.endsWith('.tsx')) {
        const content = fs.readFileSync(path.join(srcDir, file), 'utf8');
        const viewName = file.replace('.tsx', '');
        let html = convertToHtml(content);
        
        // Add Razor layout info if needed
        let razorHeader = `@{
    ViewData["Title"] = "${viewName}";
}

`;
        if (viewName === 'Login' || viewName === 'Register') {
            razorHeader = `@{
    ViewData["Title"] = "${viewName}";
    ViewData["HideNavFooter"] = true;
}

`;
        }

        fs.writeFileSync(path.join(outDir, `${viewName === 'Home' ? 'Index' : viewName}.cshtml`), razorHeader + html);
    }
});

// Convert Components
fs.readdirSync(compDir).forEach(file => {
    if (file.endsWith('.tsx')) {
        const content = fs.readFileSync(path.join(compDir, file), 'utf8');
        const viewName = file.replace('.tsx', '');
        let html = convertToHtml(content, true);
        fs.writeFileSync(path.join(sharedDir, `_${viewName}.cshtml`), html);
    }
});

console.log('Conversion done.');
