export default function PlantSelector({ value, onChange }) {
    return (
        <select value = {value} onChange={e => onChange(+e.target.value)}>
            {
                Array.from({length: 10}, (_, i) => (
                    <option key = {i + 1} value = {i + 1}>
                        Plant {i + 1}
                    </option>
                ))
            }
        </select>
    )
}