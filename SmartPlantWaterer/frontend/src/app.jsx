import { useEffect, useState } from 'react'
import PlantSelector from './components/PlantSelector'
import TelemetryCard from './components/TelemetryCard'
import TelemetryChart from './components/TelemetryChart'
import HealthWidget from './components/HealthWidget'
import { enableNotifications, notifyIfNeeded } from '../utils/notifications'

export default function App() {
  const [plantId, setPlantId] = useState(1);
  const [telemetry, setTelemetry] = useState([]);

  useEffect(() => {
    enableNotifications()
  }, [])

  //clear Telemetry when plant changes
  useEffect(() => {
    setTelemetry([])
  }, [plantId])

  //Trigger notifications when new telemetry arrives
  useEffect(() => {
    const latest = telemetry.at(-1)

    if (latest)
      notifyIfNeeded(latest)
  }, [telemetry])

  return (
    <div className="container">
      <h1>🌱 Smart Plant Waterer</h1>

      <HealthWidget plantId = {plantId} />

      <PlantSelector value={plantId} onChange={setPlantId} />

      <TelemetryCard latest={telemetry.at(-1)} />

      <TelemetryChart data={telemetry} onUpdate={setTelemetry} plantId={plantId} />
    </div>
  );
}
