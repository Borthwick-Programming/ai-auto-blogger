import { useCallback, useEffect, useRef, useState } from 'react';

/*  5 s × 1 min  →  10 s × 1 min  →  60 s × 1 min  →  stop  */
const phases = [
  { interval: 5_000,  duration: 60_000 },
  { interval: 10_000, duration: 60_000 },
  { interval: 60_000, duration: 60_000 },
];

export default function useApiStatus() {
  const [online, setOnline] = useState(true);   // optimistic first load
  const [trying, setTrying] = useState(true);

  const phaseRef     = useRef(0);               // 0,1,2
  const timeoutRef   = useRef();                // phase-switch timer
  const intervalRef  = useRef();                // ping timer

  /* ---- single ping ---- */
  const ping = useCallback(() => {
    fetch('/api/health')
      .then(r => r.ok ? setOnline(true) : setOnline(false))
      .catch(()   => setOnline(false));
  }, []);

  /* ---- schedule a whole phase ---- */
  const schedulePhase = useCallback(() => {
    if (phaseRef.current >= phases.length) {
      setTrying(false);                         // out of phases → stop
      return;
    }

    const { interval, duration } = phases[phaseRef.current];

    /* start interval for this phase */
    ping();
    intervalRef.current = setInterval(ping, interval);

    /* queue switch to next phase */
    timeoutRef.current = setTimeout(() => {
      clearInterval(intervalRef.current);
      phaseRef.current += 1;
      schedulePhase();                          // recurse
    }, duration);
  }, [ping]);

  /* ---- initial mount ---- */
  useEffect(() => {
    schedulePhase();
    return () => {                              // cleanup on unmount
      clearTimeout(timeoutRef.current);
      clearInterval(intervalRef.current);
    };
  }, [schedulePhase]);

  /* ---- manual restart ---- */
  const restart = useCallback(() => {
    clearTimeout(timeoutRef.current);
    clearInterval(intervalRef.current);
    phaseRef.current = 0;
    setTrying(true);
    schedulePhase();
  }, [schedulePhase]);

  return { online, trying, restart };
}
