import { useEffect, useState } from 'react'

export default function PlantHealthWidget({ plantId }) {
    const [health, setHealth] = useState(null)

    useEffect(() => {
        fetch(`/api/health/plants/{plantId}`)
        .then(r => r.json())
        .then(setHealth)
    }, plantId)

    if (!health) return null

    return (
        <div classname = 'card'>
            <h3>🌱 Plant {plantId}</h3>
            <p>Status: {health.status}</p>
            <p>Battery: {health.voltage ?? "N/A"} V</p>
            <p>
                Last Seen: 
                <br />
                {health.lastSeen && new Date(health.lastSeen).toLocaleString()}
            </p>
        </div>
    )
}