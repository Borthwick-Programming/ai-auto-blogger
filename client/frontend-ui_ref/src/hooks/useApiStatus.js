import { useCallback, useEffect, useRef, useState } from 'react';

const phases = [
  { interval: 5_000,  duration: 60_000 },  // 0-1 min
  { interval:10_000,  duration: 60_000 },  // 1-2 min
  { interval:60_000,  duration: 60_000 },  // 2-3 min
];

export default function useApiStatus() {
  const [online,   setOnline]   = useState(true);
  const [trying,   setTrying]   = useState(true);
  const [attempt,  setAttempt]  = useState(0);      
  const [currentMs,setCurrentMs]= useState(phases[0].interval); 

  const phaseRef    = useRef(0);
  const timeoutRef  = useRef();
  const intervalRef = useRef();

  const ping = useCallback(() => {
    setAttempt(a => a + 1);                          // count every try
    fetch('/api/health')
      .then(r => r.ok ? setOnline(true) : setOnline(false))
      .catch(()   => setOnline(false));
  }, []);

  const schedulePhase = useCallback(() => {
    if (phaseRef.current >= phases.length) {
      setTrying(false);
      return;
    }
    const { interval, duration } = phases[phaseRef.current];
    setCurrentMs(interval);                          // expose to UI

    ping();
    intervalRef.current = setInterval(ping, interval);

    timeoutRef.current = setTimeout(() => {
      clearInterval(intervalRef.current);
      phaseRef.current += 1;
      schedulePhase();
    }, duration);
  }, [ping]);

  useEffect(() => {
    schedulePhase();
    return () => { clearTimeout(timeoutRef.current); clearInterval(intervalRef.current); };
  }, [schedulePhase]);

  const restart = useCallback(() => {
    clearTimeout(timeoutRef.current); clearInterval(intervalRef.current);
    phaseRef.current = 0;
    setAttempt(0);           // reset counter
    setTrying(true);
    schedulePhase();
  }, [schedulePhase]);

  /* return the new values */
  return { online, trying, attempt, currentMs, restart };
}
