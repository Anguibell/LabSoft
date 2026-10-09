import type { ReactNode } from 'react';

interface PageContainerProps {
  children: ReactNode;
}

export default function PageContainer({ children }: PageContainerProps) {
  return (
    <main className="p-5 md:p-8 max-w-screen-2xl mx-auto w-full flex-1">
      {children}
    </main>
  );
}