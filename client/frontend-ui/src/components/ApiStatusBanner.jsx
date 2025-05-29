import './ApiStatusBanner.css';

export default function ApiStatusBanner({ online, trying, attempt, currentMs }) {
  if (online) return null;

  return (
    <div className="api-banner">
      {trying
        ? <>⚠️ Backend API is offline – attempt {attempt} every {currentMs/1000} s</>
        : <>⚠️ Backend API is offline.</>}
    </div>
  );
}