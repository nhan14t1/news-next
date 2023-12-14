import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import styles from './Footer.module.scss';
import Link from 'next/link';
import { faAddressCard } from '@fortawesome/free-solid-svg-icons';

function Footer() {
  return (
    <footer className={styles.footer}>
      <div className={`container ${styles.container}`}>
        <p className='text-center mb-0'>@ 2023 Showbiz 365</p>
      </div>
    </footer>
  );
}

export default Footer;
