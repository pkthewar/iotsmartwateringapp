import { useState } from 'react'

export default function AddPlant({ onAdded }) {
    const [name, setName] = useState('')
    const [location, setLocation] = useState('')

    const submit = async () => {
        if (!name.trim) return

        await fetch('api/plants', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json'},
            body: JSON.stringify({ name, location })
        })

        setName('')
        setLocation('')
        onAdded()
    }

    return (
        <div className = "card">
            <h3>➕ Add Plant</h3>
            
            <input placeholder = "Plant Name" value = {name} onChange = {e => setName(e.target.value)} />

            <input placeholder = "Location (optional)" value = {location} onChange = {e => setLocation(e.target.value)} />

            <button onClick={submit}>Add</button>
        </div>
    )
}