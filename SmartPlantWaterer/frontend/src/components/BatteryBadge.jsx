export default function BatteryBadge({ voltage }) {
    const color = voltage < 3.3 ? "red" : voltage < 3.6 ? "orange" : "green"
    
    return (
        <span style = {{ color }}>
           🔋 {voltage} V
        </span>
    ) 
}