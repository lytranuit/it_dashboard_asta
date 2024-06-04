<template>
  <div class="left-sidenav">
    <ul class="metismenu left-sidenav-menu">
      <li v-if="is_admin">
        <router-link class="nav-link" to="/">
          <i class="fas fa-chart-bar"></i>
          <span>Tổng quan</span>
        </router-link>
      </li>
      
      <li v-if="is_admin">
        <router-link class="nav-link" to="/history">
          <i class="fas fa-history"></i>
          <span>Audittrails</span>
        </router-link>
      </li>

      <li v-if="is_admin">
        <router-link class="nav-link" to="/user">
          <i class="fas fa-user"></i>
          <span>User</span>
        </router-link>
      </li>

    </ul>
  </div>
</template>
<script setup>
import { useAuth } from "../stores/auth";
import { onMounted, ref } from "vue";
import { useLayout } from "../layouts/composables/layout";
import { storeToRefs } from "pinia";

const { initMetisMenu, initActiveMenu } = useLayout();
const store = useAuth();
const { is_admin } = storeToRefs(store);

onMounted(() => {
  initMetisMenu();
  initActiveMenu();
});
</script>
