const API_BASE = '/api/boose';

// DOM Elements
const codeEditor = document.getElementById('codeEditor');
const runBtn = document.getElementById('runBtn');
const clearBtn = document.getElementById('clearBtn');
const canvasImage = document.getElementById('canvasImage');
const debugConsole = document.getElementById('debugConsole');
const canvasWidth = document.getElementById('canvasWidth');
const canvasHeight = document.getElementById('canvasHeight');
const exampleSelect = document.getElementById('exampleSelect');
const saveCodeBtn = document.getElementById('saveCodeBtn');
const loadCodeBtn = document.getElementById('loadCodeBtn');
const saveImageBtn = document.getElementById('saveImageBtn');
const fileInput = document.getElementById('fileInput');

// Load examples on startup
loadExamples();

// Event Listeners
runBtn.addEventListener('click', executeCode);
clearBtn.addEventListener('click', clearCanvas);
exampleSelect.addEventListener('change', loadExample);
saveCodeBtn.addEventListener('click', saveCode);
loadCodeBtn.addEventListener('click', () => fileInput.click());
saveImageBtn.addEventListener('click', saveImage);
fileInput.addEventListener('change', loadCodeFile);

// Keyboard shortcuts
codeEditor.addEventListener('keydown', (e) => {
    if (e.ctrlKey && e.key === 'Enter') {
        e.preventDefault();
        executeCode();
    }
});

async function executeCode() {
    const code = codeEditor.value;

    if (!code.trim()) {
        logDebug('No code to execute', 'error');
        return;
    }

    logDebug('Executing code...', 'info');
    runBtn.disabled = true;
    runBtn.textContent = '⏳ Running...';

    try {
        const response = await fetch(`${API_BASE}/execute`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                code: code,
                canvasWidth: parseInt(canvasWidth.value),
                canvasHeight: parseInt(canvasHeight.value)
            })
        });

        const result = await response.json();

        if (result.success) {
            canvasImage.src = `data:image/png;base64,${result.imageBase64}`;
            logDebug(`✓ Success! Executed ${result.commandCount} commands`, 'success');

            if (result.programStructure && result.programStructure.length > 0) {
                logDebug('Program structure:', 'info');
                result.programStructure.forEach(line => logDebug(`  ${line}`, 'info'));
            }
        } else {
            logDebug(`✗ Error: ${result.message}`, 'error');
            if (result.errorDetails) {
                logDebug(result.errorDetails, 'error');
            }
        }
    } catch (error) {
        logDebug(`✗ Network error: ${error.message}`, 'error');
    } finally {
        runBtn.disabled = false;
        runBtn.textContent = '▶ Run';
    }
}

async function clearCanvas() {
    codeEditor.value = '';
    debugConsole.innerHTML = '';

    try {
        const response = await fetch(`${API_BASE}/clear`, {
            method: 'POST'
        });

        const result = await response.json();
        if (result.success) {
            canvasImage.src = `data:image/png;base64,${result.imageBase64}`;
            logDebug('Canvas cleared', 'info');
        }
    } catch (error) {
        logDebug(`Error clearing canvas: ${error.message}`, 'error');
    }
}

async function loadExamples() {
    try {
        const response = await fetch(`${API_BASE}/examples`);
        const examples = await response.json();

        examples.forEach(example => {
            const option = document.createElement('option');
            option.value = example.code;
            option.textContent = example.name;
            exampleSelect.appendChild(option);
        });
    } catch (error) {
        console.error('Failed to load examples:', error);
    }
}

function loadExample() {
    const code = exampleSelect.value;
    if (code) {
        codeEditor.value = code;
        exampleSelect.value = '';
    }
}

function saveCode() {
    const code = codeEditor.value;
    const blob = new Blob([code], { type: 'text/plain' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'boose-program.txt';
    a.click();
    URL.revokeObjectURL(url);
    logDebug('Code saved', 'success');
}

function loadCodeFile(event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
            codeEditor.value = e.target.result;
            logDebug('Code loaded', 'success');
        };
        reader.readAsText(file);
    }
}

function saveImage() {
    if (!canvasImage.src || canvasImage.src === window.location.href) {
        logDebug('No image to save', 'error');
        return;
    }

    const a = document.createElement('a');
    a.href = canvasImage.src;
    a.download = 'boose-canvas.png';
    a.click();
    logDebug('Image saved', 'success');
}

function logDebug(message, type = 'info') {
    const timestamp = new Date().toLocaleTimeString();
    const entry = document.createElement('div');
    entry.className = type;
    entry.textContent = `[${timestamp}] ${message}`;
    debugConsole.appendChild(entry);
    debugConsole.scrollTop = debugConsole.scrollHeight;
}

// Initialize
logDebug('BOOSE Web Editor ready! Press Ctrl+Enter to run code.', 'success');