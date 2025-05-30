import { useState } from 'react';

export default function HttpRequestConfig({ node }) {
  const [url, setUrl]     = useState(JSON.parse(node.data.configurationJson)?.url || '');
  const [method,setMethod]= useState(JSON.parse(node.data.configurationJson)?.method || 'GET');

  return (
    <>
      <label className="field">
        URL
        <input value={url} onChange={e=>setUrl(e.target.value)} />
      </label>

      <label className="field">
        Method
        <select value={method} onChange={e=>setMethod(e.target.value)}>
          {['GET','POST','PUT','DELETE'].map(m=><option key={m}>{m}</option>)}
        </select>
      </label>

      {/* TODO wire to onSave */}
    </>
  );
}
