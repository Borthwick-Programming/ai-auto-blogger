import { useEffect, useState } from 'react';
import { getNodeDefinitions } from '../api/workflowengine';

export default function NodePalette(   { onSelect, onClose })
    { 
      const [defs, setDefs] = useState([]);
      useEffect
      (
        () => { getNodeDefinitions().then(setDefs);}, []
      ); 
     
return ( 
    <div className="palette">
    {
      defs.map
      (d => 
        (
          <button key={d.id} onClick={() => onSelect(d)}> {d.name} </button> 
        )
      )
    } 
        <button onClick={onClose}>
            ×
        </button> 
    </div> 
  ); 
    }