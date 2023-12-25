"use client"
import Link from 'next/link';
import { AdminContext } from '../../../shared/contexts/AdminContext';
import { useContext, useEffect, useState } from 'react';
import { ROLES } from '../../../shared/constants/app-const';
import { useRouter } from 'next/router';
import AuthContext from '../../../shared/contexts/AuthContext';
import AppContext from '../../../shared/contexts/AppContext';
import AppLoading from '../../shared/AppLoading';
import { Layout, Menu } from 'antd';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faNewspaper, faUser } from '@fortawesome/free-solid-svg-icons';
import styles from './AdminSidebar.module.scss';

const { Header, Content, Footer, Sider } = Layout;
const MENU_ITEMS = {
  post: {
    key: 'post',
    href: '/admin/post-management'
  },
  user: {
    key: 'user',
    href: '/admin/user-management'
  },
}

const AdminSidebar = ({ sidebarOpen, closeSidebar, children }) => {
  const router = useRouter();
  const { user, isSSR } = useContext(AuthContext);
  const { loading } = useContext(AppContext);
  const isAdmin = () => {
    return user && user[`http://schemas.microsoft.com/ws/2008/06/identity/claims/role`].includes(ROLES.Admin.Name);
  }

  const [selectedKey, setSelectedKey] = useState(MENU_ITEMS.post.key);

  useEffect(() => {
    if (!isSSR && !isAdmin()) {
      router.push('/admin/login');
    }
  }, [isSSR]);

  const activeMenuItem = () => {
    const url = location.href.toLowerCase();
    const item = Object.values(MENU_ITEMS).find(_ => url.includes(_.href)) || MENU_ITEMS.post;
    setSelectedKey(item.key);
  }

  useEffect(() => {
    activeMenuItem();
  }, [])

  return (
    <AdminContext.Provider value={{ sidebarOpen }}>
      <Layout className={styles.admin_sidebar}>
        <Sider
          breakpoint="lg"
          collapsedWidth="0"
          onBreakpoint={(broken) => {
            console.log(broken);
          }}
          onCollapse={(collapsed, type) => {
            console.log(collapsed, type);
          }}
          className={styles.sider}
        >
          <div className={styles.logo}>
            <Link href={'/'}>
              <img src='/assets/logo.svg' height={40}/>
            </Link>
          </div>
          <Menu theme="dark" className={styles.menu} selectedKeys={[selectedKey]}
            onSelect={({key}) => setSelectedKey(key)}>
            <Menu.Item key={MENU_ITEMS.post.key}>
              <FontAwesomeIcon icon={faNewspaper} />
              <Link href={MENU_ITEMS.post.href}>Bài viết</Link>
            </Menu.Item>
            <Menu.Item key={MENU_ITEMS.user.key}>
              <FontAwesomeIcon icon={faUser} />
              <Link href={MENU_ITEMS.user.href}>Người dùng</Link>
            </Menu.Item>
          </Menu>
        </Sider>

        <Layout>
          <Content>
            {children}
          </Content>
        </Layout>
      </Layout>
      <AppLoading loading={loading}></AppLoading>
    </AdminContext.Provider>
  );
}
export default AdminSidebar;