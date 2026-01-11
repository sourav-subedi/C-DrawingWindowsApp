// API endpoint
const API_URL = '/api/interpreter/execute';

// Command list
const allCommands = [
    "circle radius [true/false]",
    "rect width height [true/false]",
    "tri width height",
    "moveto x y",
    "drawto x y",
    "pen color",
    "pensize size",
    "write text",
    "clear",
    "reset",
    "int varname = value",
    "real varname = value",
    "boolean varname = value",
    "array arrayname = size",
    "poke arrayname index value",
    "peek arrayname index",
    "if condition",
    "else",
    "end if",
    "while condition",
    "end while",
    "for var = start to end [step increment]",
    "end for",
    "method [returnType] methodName [params]",
    "end method",
    "call methodName [args]"
];

// DOM Elements
const commandTextArea = document.getElementById('commandTextArea');
const singleCommandInput = document.getElementById('singleCommandInput');
const runBtn = document.getElementById('runBtn');
const runOneBtn = document.getElementById('runOneBtn');
const clearCanvasBtn = document.getElementById('clearCanvasBtn');
const clearDebugBtn = document.getElementById('clearDebugBtn');
const savePngBtn = document.getElementById('savePngBtn');
const saveCommandsBtn = document.getElementById('saveCommandsBtn');
const loadBtn = document.getElementById('loadBtn');
const fileInput = document.getElementById('fileInput');
const canvasOutput = document.getElementById('canvasOutput');
const debugBox = document.getElementById('debugBox');
const suggestionsBox = document.getElementById('suggestionsBox');

// Footer buttons
const aboutBtn = document.getElementById('aboutBtn');
const commandListBtn = document.getElementById('commandListBtn');
const documentationBtn = document.getElementById('documentationBtn');

// Modals
const commandListModal = document.getElementById('commandListModal');
const aboutModal = document.getElementById('aboutModal');

// Command normalization function (matching Form1.cs logic) 
function normalizeCommand(line) {
    const lower = line.toLowerCase().trim();

    // Normalize end if variants to "end"
    if (lower === 'end if' || lower === 'endif' || lower === 'end') {
        return 'end';
    }

    // Normalize end while
    if (lower === 'endwhile' || lower === 'end while') {
        return 'endwhile';
    }

    // Normalize end for
    if (lower === 'endfor' || lower === 'end for') {
        return 'endfor';
    }

    // Normalize end method
    if (lower === 'endmethod' || lower === 'end method') {
        return 'endmethod';
    }

    // Normalize assignments
    if (isAssignmentLine(line)) {
        return normalizeAssignment(line);
    }

    return line;
}

// Detects direct assignment
function isAssignmentLine(line) {
    const trimmed = line.trimStart();
    if (!trimmed.includes('=')) return false;

    const firstWord = trimmed.split(/\s+/)[0].toLowerCase();

    // Skip declaration keywords
    if (['int', 'real', 'array', 'boolean'].includes(firstWord)) return false;

    // Skip known commands 
    const knownCommands = ['circle', 'moveto', 'drawto', 'pen', 'rect', 'pensize',
        'tri', 'write', 'clear', 'reset', 'poke', 'peek', 'set',
        'if', 'else', 'end', 'while', 'endwhile', 'for', 'endfor',
        'method', 'endmethod', 'call'];
    if (knownCommands.includes(firstWord)) return false;

    return true;
}

// Adds "set" prefix to assignment statements 
function normalizeAssignment(line) {
    const trimmed = line.trim();

    // If it already starts with "set", return as-is 
    if (trimmed.toLowerCase().startsWith('set ')) return trimmed;

    // Add "set" prefix for BOOSE AppAsign 
    return 'set ' + trimmed;
}

// Execute commands with normalization
async function executeCommands(commands) {
    try {
        debugBox.value = 'Executing commands...\n';

        // Split and normalize each line 
        const lines = commands
            .replace(/\r/g, '')
            .split(/[\n;]/)
            .filter(l => l.trim())
            .map(line => normalizeCommand(line.trim()))
            .join('\n');

        const response = await fetch(API_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ commands: lines })
        });

        const result = await response.json();

        if (result.success) {
            canvasOutput.src = 'data:image/png;base64,' + result.image;
            debugBox.value += result.debug + '\n';
            debugBox.value += '✓ Program executed successfully!\n';
        } else {
            debugBox.value += 'Error: ' + result.error + '\n';
        }
    } catch (error) {
        debugBox.value += 'Network Error: ' + error.message + '\n';
    }
}

// Run all commands
runBtn.addEventListener('click', () => {
    const commands = commandTextArea.value;
    if (commands.trim()) {
        executeCommands(commands);
    } else {
        debugBox.value = 'No commands entered.\n';
    }
});

// Run single command 
runOneBtn.addEventListener('click', () => {
    const command = singleCommandInput.value;
    if (command.trim()) {
        executeCommands(command);
        singleCommandInput.value = '';
    } else {
        debugBox.value = 'No command entered.\n';
    }
});

// Shift + Enter to run all commands 
commandTextArea.addEventListener('keydown', (e) => {
    if (e.key === 'Enter' && e.shiftKey) {
        e.preventDefault();
        runBtn.click();
    }
});

// Clear canvas 
clearCanvasBtn.addEventListener('click', () => {
    canvasOutput.src = '';
    debugBox.value = 'Canvas cleared.\n';
});

// Clear debug 
clearDebugBtn.addEventListener('click', () => {
    debugBox.value = '';
});

// Auto-suggestions 
commandTextArea.addEventListener('input', () => {
    const input = commandTextArea.value.split('\n').pop().trim().toLowerCase();

    if (!input) {
        suggestionsBox.innerHTML = '';
        return;
    }

    const matches = allCommands.filter(cmd =>
        cmd.toLowerCase().includes(input)
    );

    if (matches.length > 0) {
        suggestionsBox.innerHTML = matches.map(cmd =>
            `<div>${cmd}</div>`
        ).join('');
    } else {
        suggestionsBox.innerHTML = '<div style="color: #999;">No matching commands</div>';
    }
});

// Save PNG only
savePngBtn.addEventListener('click', () => {
    const imgSrc = canvasOutput.src;

    if (!imgSrc || imgSrc === '') {
        debugBox.value += 'No canvas output to save. Run commands first.\n';
        return;
    }

    // Create a download link for the PNG
    const a = document.createElement('a');
    a.href = imgSrc;
    a.download = 'canvas-output.png';
    a.click();

    debugBox.value += 'Canvas saved as canvas-output.png\n';
});

// Save Commands only (.boose file) 
saveCommandsBtn.addEventListener('click', () => {
    const commands = commandTextArea.value;

    if (!commands.trim()) {
        debugBox.value += 'No commands to save.\n';
        return;
    }

    const blob = new Blob([commands], { type: 'text/plain' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'program.boose';
    a.click();
    URL.revokeObjectURL(url);

    debugBox.value += 'Commands saved as program.boose\n';
});


// Load functionality
loadBtn.addEventListener('click', () => {
    fileInput.click();
});

fileInput.addEventListener('change', (e) => {
    const file = e.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = (event) => {
            commandTextArea.value = event.target.result;
            debugBox.value += `Loaded commands from ${file.name}\n`;
            executeCommands(commandTextArea.value);
        };
        reader.readAsText(file);
    }
});

// Command List Modal 
commandListBtn.addEventListener('click', () => {
    const list = document.getElementById('commandList');
    list.innerHTML = allCommands.map(cmd =>
        `<li>${cmd}</li>`
    ).join('');
    commandListModal.style.display = 'block';
});

// About Modal 
aboutBtn.addEventListener('click', () => {
    document.getElementById('aboutContent').innerHTML = `
        <p><strong>BOOSE Interpreter</strong></p>
        <p>Version 2.01</p>
        <p>Created by Sourav Subedi</p>
        <p>Leeds Beckett University | The British College</p>
        <p>A web-based interpreter for the BOOSE drawing language.</p>
    `;
    aboutModal.style.display = 'block';
});

// Close modals 
document.querySelectorAll('.close').forEach(closeBtn => {
    closeBtn.addEventListener('click', function () {
        this.closest('.modal').style.display = 'none';
    });
});

window.addEventListener('click', (e) => {
    if (e.target.classList.contains('modal')) {
        e.target.style.display = 'none';
    }
});