import BatteryBadge from './BatteryBadge'

export default function TelemetryCard({ latest }) {
    if (!latest) return null

    return (
        <div className='card'>
            <h2>Live status:</h2>
            <p>Moisture: {latest.moisture} %</p>
            <p>Temperature: {latest.temperature} %</p>
            <p>Humidity: {latest.humidity} %</p>
            <p>Watering: {latest.waterNow ? "YES" : "NO"}</p>
            <BatteryBadge voltage = {latest.batteryVoltage}/>
        </div>
    )
}