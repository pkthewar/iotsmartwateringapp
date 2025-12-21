import {useEffect, useState} from 'react'
import * as signalR from '@microsoft/signalr'
import {LineChart, Line, XAxis, YAxis, ToolTip} from 'recharts'

export default function App() {
    const[plant, setPlant] = useState(1)
    const[data, setData] = useState([])

    useEffect(() => {
        const c = new signalR.HubConnectionBuilder().withUrl("http://localhost:5001/hubs/telemetry").build()

        c.on('TelemetryUpdate', t => {
            if (t.plantId === plant)
                setData(d => [...d.slice(-20), t])
        })

        c.start().then(() => {
            c.invoke('SubscribePlant', plant)
        })
        
        // To-Do: Add more logic in case of change in plant selection.
    }, [plant])

    return (
        <div>
            <select onChange={e => setPlant(+e.target.value)}>
                {[1,2,3,4,5].map(p => <option key = {p}>Plant {p}</option>)}
            </select>

            <LineChart width = {350} height = {250} data = {data}>
                <XAxis dataKey = "createdAt" />
                <YAxis/>
                <ToolTip/>
                <Line datakey = 'moisture' stroke = 'green'/>
            </LineChart>
        </div>
    )
}