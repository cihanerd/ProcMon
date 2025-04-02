
const API_URL = 'http://localhost:5114';

let cpuChart, memoryChart;
const chartColors = [
    'rgba(255, 99, 132, 0.7)',
    'rgba(54, 162, 235, 0.7)',
    'rgba(255, 206, 86, 0.7)',
    'rgba(75, 192, 192, 0.7)',
    'rgba(153, 102, 255, 0.7)',
    'rgba(255, 159, 64, 0.7)',
    'rgba(199, 199, 199, 0.7)',
    'rgba(83, 102, 255, 0.7)',
    'rgba(40, 159, 64, 0.7)',
    'rgba(210, 199, 199, 0.7)'
];

function createCpuChart(processes) {
    const ctx = document.getElementById('cpuChart').getContext('2d');

    const sortedProcesses = [...processes].sort((a, b) => b.cpuUsage - a.cpuUsage).slice(0, 10);

    const data = {
        labels: sortedProcesses.map(p => p.name),
        datasets: [{
            label: 'CPU Usage (%)',
            data: sortedProcesses.map(p => p.cpuUsage),
            backgroundColor: chartColors.slice(0, sortedProcesses.length),
            borderWidth: 1
        }]
    };

    if (cpuChart) {
        cpuChart.data = data;
        cpuChart.update();
    } else {
        cpuChart = new Chart(ctx, {
            type: 'bar',
            data: data,
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    y: {
                        beginAtZero: true,
                        max: 100,
                        title: {
                            display: true,
                            text: 'CPU Usage (%)'
                        }
                    }
                },
                plugins: {
                    title: {
                        display: true,
                        text: 'Top 10 CPU Usage By Process'
                    }
                }
            }
        });
    }
}

function createMemoryChart(processes) {
    const ctx = document.getElementById('memoryChart').getContext('2d');

    const sortedProcesses = [...processes].sort((a, b) => b.memoryUsage - a.memoryUsage).slice(0, 10);

    const data = {
        labels: sortedProcesses.map(p => p.name),
        datasets: [{
            label: 'Memory Usage (MB)',
            data: sortedProcesses.map(p => p.memoryUsage / (1024 * 1024)), // Convert to MB
            backgroundColor: chartColors.slice(0, sortedProcesses.length),
            borderWidth: 1
        }]
    };

    if (memoryChart) {
        memoryChart.data = data;
        memoryChart.update();
    } else {
        memoryChart = new Chart(ctx, {
            type: 'line',
            data: data,
            options: {
                animations: {
                    duration: 10,
                    easing: 'linear',
                    from: 1,
                    to: 0,
                    loop: true

                },
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'Memory Usage (MB)'
                        }
                    }
                },
                plugins: {
                    title: {
                        display: true,
                        text: 'Top 10 Memory Usage By Process'
                    }
                }
            }
        });
    }
}

function formatBytes(bytes, decimals = 2) {
    if (bytes === 0) return '0 B';
    const k = 1024;
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ['B', 'KB', 'MB', 'GB', 'TB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
}

function formatTime(dateString) {
    const date = new Date(dateString);
    return date.toLocaleTimeString() + ' ' + date.toLocaleDateString();
}

function updateProcessTable(processes) {
    const tableBody = document.getElementById('processTableBody');
    tableBody.innerHTML = '';

    processes.forEach(process => {
        const row = document.createElement('tr');
        if (process.isImportant) {
            row.classList.add('process-important');
        }

        row.innerHTML = `
             <td>${process.id}</td>
             <td>${process.name}</td>
             <td>${process.cpuUsage.toFixed(1)}%</td>
             <td>${formatBytes(process.memoryUsage)}</td>
             <td>${process.status}</td>
             <td>${formatTime(process.startTime)}</td>
             <td>${process.threadCount}</td>
             <td>${process.isImportant ? '✓' : ''}</td>
         `;

        tableBody.appendChild(row);
    });
}

async function getMonitoringStatus() {
    try {
        const response = await fetch(`${API_URL}/api/process/monitoring-status`);
        const data = await response.json();

        const statusElement = document.getElementById('monitoringStatus');
        if (data.isMonitoring) {
            statusElement.textContent = 'Active';
            statusElement.classList.remove('status-inactive');
            statusElement.classList.add('status-active');
        } else {
            statusElement.textContent = 'Inactive';
            statusElement.classList.remove('status-active');
            statusElement.classList.add('status-inactive');
        }
    } catch (error) {
        console.error('Error getting monitoring status:', error);
    }
}

async function startMonitoring() {
    try {
        await fetch(`${API_URL}/api/process/start-monitoring`, {
            method: 'POST'
        });
        getMonitoringStatus();
    } catch (error) {
        console.error('Error starting monitoring:', error);
    }
}

async function stopMonitoring() {
    try {
        await fetch(`${API_URL}/api/process/stop-monitoring`, {
            method: 'POST'
        });
        getMonitoringStatus();
    } catch (error) {
        console.error('Error stopping monitoring:', error);
    }
}

async function refreshData() {
    try {
        const showImportantOnly = document.getElementById('showImportantOnly').checked;
        const endpoint = showImportantOnly ? `${API_URL}/api/process/important` : `${API_URL}/api/process`;

        const response = await fetch(endpoint);
        const processes = await response.json();

        updateProcessTable(processes);
        createCpuChart(processes);
        createMemoryChart(processes);
    } catch (error) {
        console.error('Error refreshing data:', error);
    }
}

function initializeSignalR() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl(`${API_URL}/processhub`)
        .withAutomaticReconnect()
        .build();

    connection.on('ReceiveProcesses', (processes) => {
        updateProcessTable(processes);
        createCpuChart(processes);
        createMemoryChart(processes);
    });

    connection.on('ReceiveNotification', (notification) => {
        alert(notification.message);
    });

    connection.start()
        .then(() => console.log('SignalR Connected'))
        .catch(err => console.error('SignalR Connection Error: ', err));
}

async function initialize() {
    document.getElementById('startMonitoringBtn').addEventListener('click', startMonitoring);
    document.getElementById('stopMonitoringBtn').addEventListener('click', stopMonitoring);
    document.getElementById('refreshBtn').addEventListener('click', refreshData);
    document.getElementById('showImportantOnly').addEventListener('change', refreshData);

    await getMonitoringStatus();
    await refreshData();

    initializeSignalR();

}

document.addEventListener('DOMContentLoaded', initialize);