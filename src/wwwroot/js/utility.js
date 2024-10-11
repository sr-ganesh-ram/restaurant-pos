let idleTime = 0;
let idleInterval = null;

function resetTimer() {
    idleTime = 0;
    clearInterval(idleInterval);
    idleInterval = setInterval(() => {
        idleTime += 1;
        if (idleTime >= 60) { // 1 minute of inactivity
            alert('You have been inactive for 1 minute.');
            // Call Blazor method to handle idle time
            DotNet.invokeMethodAsync('YourBlazorComponent', 'HandleIdleTime');
        }
    }, 60000); // 1 minute interval
}