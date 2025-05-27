import { useEffect, useRef, useState } from 'react';

/**
 * Polls /api/health every {intervalMs}. Returns { online: boolean }.
 */
export default function useApiStatus(intervalMs = 4000) {
  const [online, setOnline] = useState(true);     // assume up on first load
  const timer = useRef(null);

  useEffect(() => {
    const ping = () =>
      fetch('/api/health')
        .then(r => r.ok ? setOnline(true) : setOnline(false))
        .catch(() => setOnline(false));

    ping();                             // immediate
    timer.current = setInterval(ping, intervalMs);

    return () => clearInterval(timer.current);
  }, [intervalMs]);

  return { online };
}
