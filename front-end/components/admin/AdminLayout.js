import AdminSidebar from './Sidebar/AdminSidebar';

function AdminLayout({ children }) {
  return (
    <div className='admin-layout'>
      <AdminSidebar sidebarOpen={true} openSidebar={() => {}} />
      {children}
    </div>
  );
}

export default AdminLayout;
