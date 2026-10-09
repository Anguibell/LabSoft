import AppRoutes from './routes/AppRoutes';
import AppProviders from './app/AppProviders';

function App() {
  return (
    <AppProviders>
      <AppRoutes />
    </AppProviders>
  );
}

export default App;