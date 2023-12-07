import Link from 'next/link';
import { AdminContext } from '../../../shared/contexts/AdminContext';
import { useContext } from 'react';
import { ROLES } from '../../../shared/constants/app-const';
import { useRouter } from 'next/router';
import AuthContext from '../../../shared/contexts/AuthContext';
import AppContext from '../../../shared/contexts/AppContext';
import AppLoading from '../../shared/AppLoading';
const AdminSidebar = ({ sidebarOpen, closeSidebar }) => {
  const { user } = useContext(AuthContext);
  const { loading } = useContext(AppContext);
  const isAdmin = () => {
    return user && user[`http://schemas.microsoft.com/ws/2008/06/identity/claims/role`].includes(ROLES.Admin.Name);
  }

  if (typeof window == 'object' && !isAdmin()) {
    const router = useRouter();
    router.push('/admin/login');
    return <></>;
  }

  return (
    <AdminContext.Provider value={{ sidebarOpen }}>
      <AppLoading loading={loading}></AppLoading>

      <div className={sidebarOpen ? 'sidebar-responsive' : ''} id="sidebar">
        <div className='sidebar__title'>
          <div className='sidebar__img'>
            <h1>Showbiz365 Admin</h1>
          </div>

          <i
            onClick={() => closeSidebar()}
            className="fa fa-times"
            id="sidebarIcon"
            aria-hidden="true"
          ></i>
        </div>

        <div className="sidebar__menu">
          <div className="sidebar__link active_menu_link">
            <i className="fa fa-minus-square"></i>
            <Link href="/admin/post-management">Home</Link>
          </div>

          <div className="sidebar__link">
            <i className="fa fa-tachometer"></i>
            <Link href="/admin/post-management">Bài viết</Link>
          </div>
          <div className="sidebar__link">
            <i className="fa fa-building"></i>
            <Link href="/admin/user-management">Người dùng</Link>
          </div>
          <div className="sidebar__logout">
            <i className="fa fa-power-off"></i>
            <a href="#">Log out</a>
          </div>
        </div>
      </div>
    </AdminContext.Provider>
  );
}
export default AdminSidebar;