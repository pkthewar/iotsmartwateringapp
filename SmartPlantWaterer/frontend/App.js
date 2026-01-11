import {useEffect, useRef, useState} from 'react'
import * as signalR from '@microsoft/signalr'
import {LineChart, Line, XAxis, YAxis, ToolTip} from 'recharts'
import AddPlant from './src/components/AddPlant'

export default function App() {
    const[plants, setPlants] = useState([])
    const[plant, setPlant] = useState(null)
    const[data, setData] = useState([])
    const connectionRef = useRef(null)

    const reloadPlants = () => {
        fetch(`/api/plants`)
        .then(r => r.json())
        .then(p => {
            setPlants(p)

            if (!plant && p.length)
                setPlant(p[0].id)
        })
    }

    //Load plants dynamically
    useEffect(() => {
        fetch(`/api/plants`)
        .then(r => r.json())
        .then(p => {
            setPlants(p)
            if(p.length > 0)
                setPlant(p[0].id) //auto-select first plant
        })
    })

    //SignalR subscription per plant
    useEffect(() => {
        if(!plant) return

        setData([])

        const connection = new signalR.HubConnectionBuilder().withUrl('http://localhost:5001/hubs/telemetry').withAutomaticReconnect().build()

        connection.on('TelemetryUpdate', t => {
            if (t.plantId === plant)
                setData(d => [...d.slice(-20), t])
        })

        connection.start()
        .then(() => connection.invoke('SubscribePlant', plant))
        .catch(console.error)

        connectionRef.current = connection

        return() => {
            if (connection.state === signalR.HubConnectionState.Connected)
                connection.invoke('UnsubscribePlant', plant).catch(() => {})

            connection.stop()
        }
    }, [plant])

    return (
        <div>
            <h2>🌱 Smart Plant Dashboard</h2>

            <AddPlant onAdded = {reloadPlants} />

            <select value = {plant ?? ''} onchange = {e => setPlant(Number(e.target.value))}>
                {plants.map(p => (
                    <option key = {p.id} value = {p.value}>
                        {p.name ?? `Plant-${p.id}`}
                    </option>
                ))}
            </select>

            <LineChart width = {350} height = {250} data = {data}>
                <XAxis dataKey = "createdOn"/>
                <YAxis />
                <ToolTip />
                <Line dataKey = "moisture" stroke = "green" />
            </LineChart>
        </div>
    )
}