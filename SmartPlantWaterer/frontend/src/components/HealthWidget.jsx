import { useEffect, useState } from "react";

export default function HealthWidget({ plantId }) {
    const [systemHealth, setSystemHealth] = useState(null)
    const [plantHealth, setPlantHealth] = useState(null) 

    // Load System Health (only once)
    useEffect(() => {
        fetch(`/api/health/getHealth`)
        .then(r => r.json())
        .then(setHealth);
    }, []);

    //Load Plant Health (on plant change)
    useEffect(() => {
        if(!plantId) return

        fetch(`/api/health/plants/${plantId}`)
        .then(r => r.json())
        .then(setPlantHealth)
        .catch(() => setPlantHealth(null))
    })

    if (!systemHealth) return null;

    return (
    <div className="card">

        {/* ------- SYSTEM HEALTH -------  */}

        <h2>🩺 System Health</h2>

        <p>Status: <b>{systemHealth.overallStatus}</b></p>
        <p>Database: {systemHealth.isDatabaseRunning ? "OK" : "DOWN"}</p>
        <p>MQTT: {systemHealth.isMqttWorking ? "OK" : "DOWN"}</p>
        <p>ONNX: {systemHealth.isOnnxPredicting ? "OK" : "DOWN"}</p>

        <p>
            Plants Active: {systemHealth.activePlants}/{systemHealth.totalPlants}
        </p>

        <p>
            Last Telemetry:
            <br />
            {systemHealth.lastTelemetryTime &&
            new Date(systemHealth.lastTelemetryTime).toLocaleString()}
        </p>

        {/* ---------- PLANT HEALTH ---------- */}
        {plantHealth && (
            <>
                <hr />

                <h3>Plant Health</h3>

                <p>
                    Battery Voltage: { " " }
                    <b>{plantHealth.voltage?.toFixed(2)} V</b>
                </p>

                <p>Status: {plantHealth.status}</p>

                <p>
                    Last Alert:
                    < br/>
                    {plantHealth.lastAlert || 'None'}
                </p>
            </>
        )}        
        
    </div>
  );
}
