import '../styles/globals.scss';
import '../styles/admin/admin-sidebar.css';
import Layout from '../components/Layout/Layout';

import Router from 'next/router';
import NProgress from 'nprogress';
import 'nprogress/nprogress.css';
import 'react-quill/dist/quill.snow.css';
import 'antd/dist/reset.css';
import 'bootstrap/dist/css/bootstrap.min.css';

// NProgress Customization
NProgress.configure({
  minimum: 0.3,
  easing: 'ease',
  speed: 800,
  showSpinner: false,
});

// Show a loading bar when changing routes
Router.events.on('routeChangeStart', () => NProgress.start());
Router.events.on('routeChangeComplete', () => NProgress.done());
Router.events.on('routeChangeError', () => NProgress.done());

function MyApp({ Component, pageProps }) {
  const getLayout = Component.getLayout || ((page) => <Layout>{page}</Layout>);
  
  return getLayout(<Component {...pageProps} />);
}

export default MyApp;
