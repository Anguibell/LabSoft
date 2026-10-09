import { Outlet } from 'react-router-dom';
import Sidebar from './Sidebar';
import Header from './Header';
import PageContainer from './PageContainer';

export default function MainLayout() {
  return (
    <div className="flex w-full min-h-screen bg-[#f3f7fb] text-[#15243b]">
      <Sidebar />
      <div className="flex-1 flex flex-col min-w-0">
        <Header />
        <PageContainer>
          {/* Outlet es el marcador de posición donde React Router inyectará la página activa */}
          <Outlet />
        </PageContainer>
      </div>
    </div>
  );
}