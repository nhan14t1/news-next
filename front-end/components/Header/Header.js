import Image from 'next/legacy/image';
import Link from 'next/link';
import styles from './Header.module.scss';
import { SearchBar } from '../index';

function Header(props) {
  return (
    <header className={styles.header}>
      <div className={styles.container}>
        <div className={styles.logo}>
          <Link href='/' onClick={props.onClick}>
            <Image
              src='/assets/KaiOS-Logo.svg'
              alt='KaiOS Logo'
              width={142}
              height={56}
            />
          </Link>
        </div>
        <div className={styles.search}>
          <SearchBar {...props} />
        </div>
      </div>
    </header>
  );
}

export default Header;
