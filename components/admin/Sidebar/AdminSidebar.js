import Link from 'next/link';

const AdminSidebar = ({ sidebarOpen, closeSidebar }) => {
  return (
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
  )
}
export default AdminSidebar;