import type { ReactNode } from 'react'; 

interface PageHeaderProps {
  title: string;
  description?: string;
  action?: ReactNode;
}

export default function PageHeader({ title, description, action }: PageHeaderProps) {
  return (
    <div className="flex flex-col md:flex-row md:items-center justify-between mb-6 gap-4">
      <div>
        <h1 className="text-2xl font-bold text-[#15243b]">{title}</h1>
        {description && (
          <p className="text-[#64768b] mt-1 text-sm">{description}</p>
        )}
      </div>
      {action && (
        <div className="shrink-0"> 
          {action}
        </div>
      )}
    </div>
  );
}