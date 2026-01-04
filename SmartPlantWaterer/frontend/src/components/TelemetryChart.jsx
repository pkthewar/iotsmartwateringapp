import { useEffect } from 'react'
import { connectToPlant } from 'signalr'
import { notify } from '../utils/notifications'

const MOISTURE_CRITICAL = 20
const BATTERY_LOW = 3.2

export default function TelemetryChart( { data, onUpdate, plantId }) {
    useEffect(() => {
        const conn = connectToPlant(plantId, t => {
            
            if (t.moisture !== undefined && t.moisture < MOISTURE_CRITICAL) {
                notify(`🌱 Plant ${plantId} needs water`, `Moisture dropped to ${t.moisture} %`)
            }

            if (t.voltage !== undefined && t.voltage < BATTERY_LOW) {
                notify(`Plant ${plantId}'s battery is low`, `Battery is at ${t.voltage} V`)
            }

            onUpdate(prev => [...prev.slice(-50), t])
        })

        return () => conn.stop()
    }, [plantId, onUpdate])

    return (
        <div classname = "chart">
            {data.map((d,i) =>(
                <div key = {i}>
                    {d.timestamp} → {d.moisture}%
                    {d.voltage && `| ${d.voltage} V`}
                </div>
            ))}
        </div>
    )
}