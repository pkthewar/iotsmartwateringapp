import {useEffect, useState} from 'react'
import * as signalR from '@microsoft/signalr'
import {LineChart, Line, XAxis, YAxis, ToolTip} from 'recharts'

export default function App() {
    const[plant, setPlant] = useState(1)
    const[data, setData] = useState([])
    const connectionRef = useState(null)

    //Clear old telemetry
    useEffect(() => {
        setData([])
    
        if (connectionRef.current)
            connectionRef.current.stop()

        const connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:5001/hubs/telemetry").withAutomaticReconnect().build()

        connection.on('TelemetryUpdate', t => {
            if (t.plantId === plant)
                setData(d => [...d.slice(-20), t])
        })

        connection.start().then(() => {
            connection.invoke('SubscribePlant', plant)
        })

        connectionRef.current = connection


        return() => {
            connection.stop()
        }
    }, [plant])

    return (
        <div>
            <select value = {plant} onChange={e => setPlant(+e.target.value)}>
                {[1,2,3,4,5,6,7,8,9,10].map(p => <option key = {p}>Plant {p}</option>)}
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